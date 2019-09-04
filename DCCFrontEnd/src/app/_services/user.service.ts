import { map } from 'rxjs/operators';
import { PaginatedResult, Pagination } from './../_models/pagination';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { Message } from '../_models/Message';
 
@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>('/api/users');
  }
  getUser(id): Observable<User> {
    return this.http.get<User>(`/api/users/${id}`);
  }
  updateUser(id: number, user: User) {
    return this.http.put(`/api/users/${id}`, user);
  }
  setMainPhoto(userId: number, photoId: number) {
    return this.http.post(`/api/users/${userId}/photos/${photoId}/setMain`, {});
  }
  deletPhoto(userId, photoId: number) {
    return this.http.delete(`/api/users/${userId}/photos/${photoId}`);
  }
  getUserPaging(page?, itemsPerPage?, userParams?, likesParam?): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    if (userParams != null) {
      params = params.append('orderBy', userParams.OrderBy)
      params = params.append('specialization', userParams.Specialization)

    }

    if (likesParam === 'Likers') {
      params = params.append('likers', 'true');
    }
    if (likesParam === 'Likees') {
      params = params.append('likees', 'true');
    }
    return this.http.get<User[]>(`api/users/getPaging`, { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagintion = JSON.parse(response.headers.get('Pagination'))
            return paginatedResult;
          }
        })
      );
  }
  sendLike(id, recipientId) {
    return this.http.post(`/api/users/${id}/like/${recipientId}`, {});
  }
  getspe() {
    return this.http.get('/api/Users/getspecil');
  }
  getMessages(id, page?, itemsPerPage?, messageContainer?) {
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();
    let params = new HttpParams();
    params = params.append('MessageContainer', messageContainer)
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this.http.get<Message[]>(`/api/users/${id}/messages`,{observe: 'response', params})
    .pipe(
      map(response =>{
        paginatedResult.result = response.body;

        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagintion = JSON.parse(response.headers.get('Pagination'));
          return paginatedResult;
        }
      })
    );
  }
  getMessageThread(id, recipientId: number) {
    return this.http.get<Message[]>(`/api/users/${id}/Messages/thread/${recipientId}`);
  }

  sendMessage(id , message: Message) {
    return this.http.post(`/api/users/${id}/Messages`, message);
  }
  deleteMessage(id , userId){
    return this.http.post(`/api/users/${userId}/messages/${id}`,{})
  }
  markAsRead(userId  , messageId: number) {
    this.http.post(`/api/users/${userId}/messages/${messageId}/read`, {}).subscribe();
  }
}
