import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from './../_services/Auth.service';
import { UserService } from 'src/app/_services/user.service';
import { Pagination, PaginatedResult } from './../_models/pagination';
import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/Message';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pagination: Pagination;
  messageContainer = 'Inbox';
  constructor(private userServce: UserService,
    private authService: AuthService,
    private route: ActivatedRoute, private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      
      this.messages = data['messages'].result ;
      console.log(this.messages, 'messages')
      //this.pagination = data['messages'].pagination;
      this.pagination = data['messages'].pagintion;

      console.log(this.pagination, 'messages')

    });
  }
  pageChanged(event: any ):void {
    this.pagination.currentPage = event.page;
    this.loadMessages();
  }
  deleteMessage(id ){
    this.alertify.confirm('Are you sure want to delete this message ?' , () => {
      this.userServce.deleteMessage(id, this.authService.decodedToken.nameid).subscribe(() => {
        this.messages.splice(this.messages.findIndex( m=> m.id === id), 1);
      }, err => {
        this.alertify.error('Failed to delete the message');
      })
    })
  }
loadMessages(){
  this.userServce.getMessages(this.authService.decodedToken.nameid,
    this.pagination.currentPage,
    this.pagination.itemPerPage,
    this.messageContainer).subscribe((res: PaginatedResult<Message[]>) => {
      this.messages = res.result;
      this.pagination = res.pagintion;
    }, err => {
      this.alertify.error(err);
    }
    )
}

}
