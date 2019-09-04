import { AuthService } from './../../_services/Auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from './../../_services/user.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/_models/User';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { TabsetComponent } from 'ngx-bootstrap';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-Doctor-Details',
  templateUrl: './Doctor-Details.component.html',
  styleUrls: ['./Doctor-Details.component.css']
})
export class DoctorDetailsComponent implements OnInit {
user: User ;
placeHolder='../../../assets/user.png';
@ViewChild('memberTabs') memberTabs: TabsetComponent;
galleryOptions: NgxGalleryOptions[];
galleryImages: NgxGalleryImage[];
  constructor(private userService: UserService ,
     private alert: AlertifyService,
     private authService:AuthService,
      private rout: ActivatedRoute) { }

  ngOnInit() {
    this.rout.data.subscribe( data => {
      this.user = data['user'];
    });
    this.rout.queryParams.subscribe(params => {
      const selectedTab = params['tab'];
      this.memberTabs.tabs[selectedTab > 0 ? selectedTab : 0].active = true;
    });
    this.galleryOptions = [{
      width: '500px',
      height: '500px',
      imagePercent: 100 ,
      thumbnailsColumns: 4 ,
      imageAnimation: NgxGalleryAnimation.Slide,
      preview: false
    }
  ];
  this.galleryImages = this.getImage();

  }
  getImage() {
    const imgUrls = [] ;
    for (let i = 0; i < this.user.photos.length; i++ ) {
      imgUrls.push({
        small: this.user.photos[i].url ,
        medium: this.user.photos[i].url ,
        big: this.user.photos[i].url ,
        description: this.user.photos[i].description

      });
    }
    return imgUrls ;
  }
  sendLike(id ){
    this.userService.sendLike(this.authService.decodedToken.nameid,id).subscribe(data => {
      this.alert.success('You Have Liked:' + this.user.knownAs);
    },error =>{
      console.log(error);
      this.alert.error(error);
    })
  }
selectTab(tabId: number) {
  this.memberTabs.tabs[tabId].active = true
}
}
