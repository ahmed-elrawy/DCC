import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_models/Photo';
import { FileUploader } from 'ng2-file-upload';
import { AuthService } from 'src/app/_services/Auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
 
@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  loading = true ;
@Input() photos: Photo[] ;
@Output() getMemberChange = new EventEmitter<string>();
  uploader:FileUploader ;
  hasBaseDropZoneOver = false;
  uploadClicked = false ;
  currentPhoto : Photo ;
  photoUrl: string ;

  constructor(private auth: AuthService, private userService:UserService, private alert: AlertifyService) { }

  ngOnInit() {
     this.initializeUploader();
  }
public fileOverBase(e: any) : void {
  this.hasBaseDropZoneOver = e;
}
initializeUploader() {
  this.uploader = new FileUploader({
    url: `http://localhost:5000/api/users/${this.auth.decodedToken.nameid}/photos`,
    authToken: 'Bearer ' + localStorage.getItem('token') ,
    isHTML5: true ,
    allowedFileType: ['image'] ,
    removeAfterUpload: true ,
    autoUpload: false ,
    maxFileSize: 10 * 1024 * 1024
  });
   this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };
  this.uploader.onSuccessItem = (item, response , status, headers) => {
    if (response)
    {
      const res: Photo = JSON.parse(response);
      const photo = {
        id: res.id ,
        url: res.url ,
        dateAdded: res.dateAdded ,
        description: res.description,
        isMain:  res.isMain
      };
      this.photos.push(photo) ;
      if (photo.isMain){
        this.auth.changeMemberPhoto(photo.url);
    this.auth.currentUser.photoUrl =photo.url;
    localStorage.setItem('currentUser',JSON.stringify(this.auth.currentUser));
      }
    }
  };
 
}
setMainPhoto(photo: Photo) {
  this.userService.setMainPhoto(this.auth.decodedToken.nameid , photo.id).subscribe((res) => {
    this.currentPhoto = this.photos.filter(p => p.isMain)[0] ;
    this.currentPhoto.isMain = false ;
    photo.isMain = true ;
    this.auth.changeMemberPhoto(photo.url);
    this.auth.currentUser.photoUrl =photo.url;
    localStorage.setItem('currentUser',JSON.stringify(this.auth.currentUser));
    this.alert.success("Main Photo Updated Successfully !"),
    console.log("Main Photo Updated Successfully");
  },
  (err) => {
    this.alert.error(err) ;
  }
  )
}
deletePhoto(photoId) {
  this.alert.confirm('Are You Sure You Want Delete This Photo ? ' ,()=>{
    this.userService.deletPhoto(this.auth.decodedToken.nameid , photoId).subscribe((res) => {
      this.photos.splice(this.photos.findIndex(p => p.id === photoId),1);
      this.alert.success('Photo Has Been Deleted !');
    },
    (err) => {
      this.alert.error(err);
    });
  })
 
}

 }
