import { AuthService } from './../../_services/Auth.service';
import { BehaviorSubject } from 'rxjs';
import { Pagination, PaginatedResult } from './../../_models/pagination';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { User } from 'src/app/_models/User';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'app-DoctorsList',
  templateUrl: './DoctorsList.component.html',
  styleUrls: ['./DoctorsList.component.css']
})
export class DoctorsListComponent implements OnInit {
users ;
user: User = JSON.parse(localStorage.getItem('user'));
userParams: any = {};
likeParam: string ;
pagination: Pagination ;
likers ;
loading = true ;
data = new BehaviorSubject<User>(this.users);
Speclook ;
  constructor(private $userService: UserService,
    public auth: AuthService,
     private alert: AlertifyService, private rout: ActivatedRoute) { }

  ngOnInit() {
    this.$userService.getspe().subscribe((data: any []) => {
      this.Speclook = data.filter(x => x != null );
      })
    this.rout.data.subscribe(data => {
      this.data.next(data['users'].result)  
      this.users = this.data;
      this.pagination = data['users'].pagintion;
      });
     this.userParams.OrderBy = 'lastActive';
     this.userParams.likees = 'likees';
     this.userParams.Specialization = "" ;

     
     this.getDoctors();

   }
   pageChanged(event : any): void{
    this.pagination.currentPage = event.page
    console.log(this.pagination)
     this.getDoctors();
    }
getDoctors() {
   this.$userService.getUserPaging(this.pagination.currentPage , this.pagination.itemPerPage,this.userParams)
  .subscribe((resp : PaginatedResult<User[]>) => {
    this.users = resp.result ;
    this.pagination = resp.pagintion
    this.loading = false
  },
  (err) => {
    this.alert.error(err);
  });
}
refreshData(){
  this.rout.data.subscribe(data => {
    this.data.next(data['users'].result)  })
    this.getDoctors();
}
isDoctor(){
   
  console.log(this.auth.currentUser.typeOfUser)
 if (this.auth.currentUser.typeOfUser === "Doctors"){
   return true ;
 }else {
   return false ;
 }
}
}
