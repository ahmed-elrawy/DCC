import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from './../../_services/Auth.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/User';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-member-profile-edit',
  templateUrl: './member-profile-edit.component.html',
  styleUrls: ['./member-profile-edit.component.css'],
  providers: [DatePipe]
})
export class MemberProfileEditComponent implements OnInit {
  loading = true;
  @ViewChild('editForm') editForm: NgForm ;
user: User;
dateFormate ;
placeHolder='../../../assets/user.png';

photoUrl: string ;
@HostListener('window:beforeunload', ['$event'])
unloadNotification($event: any) {
  if (this.editForm.dirty) {
    $event.returnValue = true ;
  }
}
  constructor(private rout: ActivatedRoute,
     private authService: AuthService,
     private datepipe: DatePipe,
      private userService: UserService ,
      private alert: AlertifyService ) {
       }

  ngOnInit() {
    this.rout.data.subscribe(data => {
      this.user = data['user'];
      this.loading = false ;
      this.dateFormate = this.datepipe.transform(this.user.dateOfBirth, 'dd/MM/yyy');
      console.log(this.dateFormate);
     });
     this.authService.currentPhotourl.subscribe(photourl => this.photoUrl = photourl);
  }

updateUser() {

  this.user.dateOfBirth = this.dateFormate ;
   this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
    console.log(this.user.dateOfBirth);

    this.alert.success('Profile Updated Successfully');
    this.editForm.reset(this.user);

  }, (err) => {
    this.alert.error(err);
  });
}
updateMainPhoto(photoUrl) {
this.user.photoUrl = photoUrl ;
}
}
