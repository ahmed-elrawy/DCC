import { AuthService } from 'src/app/_services/Auth.service';
import { Subscription, BehaviorSubject, Subject, Observable } from 'rxjs';
import { Injectable, ViewChild, EventEmitter } from '@angular/core';
import *  as signalR from '@aspnet/signalr';
import { Message } from '../_models/Message';
import { MemberMessagesComponent } from '../members/member-messages/member-messages.component';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  public mes: Message;
   ;
  constructor(public auth : AuthService) { }

getToken() {
  let token = localStorage.getItem("token");
return token ;
}
loggedIn() {
  return this.auth.loggedIn();
}

   private hubConnection: signalR.HubConnection;
  public _data: BehaviorSubject<any[]> = new BehaviorSubject<any>([]);
  notification: boolean = false;

 public _notification: BehaviorSubject<boolean>= new BehaviorSubject<boolean>(this.notification);
  public flag = this._notification.asObservable();

 public currentMessage = this._data.asObservable();
  @ViewChild(MemberMessagesComponent) membMessage: MemberMessagesComponent;
  public messageReceived = new EventEmitter<any>();  
   

  public startConnection = () => {
    if(this.loggedIn()){
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5000/chats", { accessTokenFactory :() => this.getToken() }).build();


    this.hubConnection
      .start()
      .then(() => console.log('Connection started from here'))
      .catch(err => console.log('Error while starting connection: ' + err))
 
    this.hubConnection.on("ReceiveMessage", (message: Message) => {
       this.messageReceived.emit(message)
      this.updatemessage(message)
      if (message.id != null) {
      this.setPadg(true);
console.log(this.notification);
        
      }
       console.log(message)
         });

         this.hubConnection.invoke("ReceiveMessage", (message: Message) => {
          this.messageReceived.emit(message)
         this.updatemessage(message)
          console.log(message)
            });
            this.hubConnection.onclose(() => {
              this.hubConnection = new signalR.HubConnectionBuilder()
              .withUrl("http://localhost:5000/chats", { accessTokenFactory :() => this.getToken() }).build();
        
            })
        }else {
          return "err" ;
        }
  }

updatemessage(message) {
 this._data.next(message);
}
setPadg(value: boolean) {
  this._notification.next(value);
  this.notification = value;
}

}
