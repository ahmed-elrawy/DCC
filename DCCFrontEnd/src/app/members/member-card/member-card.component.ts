import { AuthService } from './../../_services/Auth.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { User } from 'src/app/_models/User';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() user: User;
  @Output() liked = new EventEmitter<boolean>();
  @Input() likerLength: number;
  userDetail: User;
  placeHolder = '../../../assets/user.png';

  constructor(private $userService: UserService,
    private alert: AlertifyService,
    private autService: AuthService) { }

  ngOnInit() {
    if (this.user.photoUrl == null) {
      this.user.photoUrl == this.placeHolder;
    }
  }
  sendLike(id ){
    this.$userService.sendLike(this.autService.decodedToken.nameid,id).subscribe(data => {
      this.alert.success('You Have Liked:' + this.user.knownAs);
      this.liked.emit(true);
    },error =>{
      console.log(error);
      this.alert.error(error);
    })
  }
  

}
