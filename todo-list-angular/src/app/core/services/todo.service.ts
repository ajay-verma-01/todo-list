import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Errors, Todo } from '../models';
import { ApiService } from './api.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root',
})
@Injectable()
export class TodoService {
  constructor(
    private apiService: ApiService,
    private userService: UserService
  ) {}

  getAll(): Observable<Todo[]> {
    return this.apiService.get(`/todolist`).pipe(map((data) => data));
  }

  getByUserId(): Observable<Todo[]> {
    return this.apiService.get(`/todolist/getbyuser`).pipe(map((data) => data));
  }

  save(payload: Todo): Observable<Todo> {
    return this.apiService.post(`/todolist`, payload).pipe(map((data) => data));
  }

  update(payload: Todo): Observable<Todo> {
    return this.apiService
      .put(`/todolist/id`, { body: payload })
      .pipe(map((data) => data));
  }

  delete(id: number): Observable<any> {
    return this.apiService.delete(`/todolist/${id}`).pipe(map((data) => data));
  }

  patch(id: number, isCompleted: number): Observable<Todo> {
    return this.apiService
      .patch(`/todolist/${id}`, isCompleted)
      .pipe(map((data) => data));
  }
}
