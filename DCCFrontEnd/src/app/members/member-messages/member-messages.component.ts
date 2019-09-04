import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/Auth.service';
import { UserService } from './../../_services/user.service';
import { Component, OnInit, Input } from '@angular/core';
import { Message } from 'src/app/_models/Message';
import { Subscription, Observable, BehaviorSubject } from 'rxjs';
import { SignalRService } from 'src/app/_services/signalR.service';
import { FormBuilder, Validators } from '@angular/forms';
import *  as signalR from '@aspnet/signalr';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @Input() recipientId: number;
  notificationSubscription: Subscription;

  public messages: any[] = [];
  public testingMessage: any[] = [];

  newMessage: any = { content: '' };
  _hubConnection: HubConnection;
  public async: any;
  selectedOnlineUserName = '';
  dmStateSubscription: Subscription | undefined;
  isAuthorizedSubscription: Subscription | undefined;
  isAuthorized = false;
  connected = false;
  public message;
  messageForm = this.fb.group({
    message: ['Hello world!', Validators.required]
  });

  constructor(private userService: UserService,
    private authService: AuthService,
    private alertify: AlertifyService,

    private signl: SignalRService,
    private fb: FormBuilder
  ) {


  }
  ngOnInit() {
    this.signl._data.subscribe((r: any) => {
      this.message = r;
      console.log(r);
      this.messages.push(this.message);
      this.loadMessages();
      this.signl.setPadg(false);

    })
  }


  loadMessages() {
    const currentUserId = this.authService.decodedToken.nameid;
    this.userService.getMessageThread(this.authService.decodedToken.nameid, this.recipientId)
      .pipe( tap(messages => {
        
        for (let i = 0; i < messages.length; i++) {
          if (messages[i].isRead === false && messages[i].recipientId === currentUserId) {
            this.userService.markAsRead(currentUserId, messages[i].id)
          }

        }
      }))
      .subscribe((r) => {
        this.messages = r
        console.log(r['result'])
      })

  }



  sendMessage() {
     this.signl.updatemessage(this.newMessage);
    this.newMessage.recipientId = this.recipientId;
    // this.signl.send(this.messageForm.getRawValue().message);
    this.userService.sendMessage(this.authService.decodedToken.nameid,
      this.newMessage).subscribe((message: Message) => {
        this.messages.unshift(message);
        this.signl.updatemessage(message);
        this.newMessage.content = '';
      }, error => {
        this.alertify.error(error);
      }
      )
   }

}
