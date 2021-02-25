import { Component, OnInit } from '@angular/core';
import { throwError } from 'rxjs';
import { Errors, Todo } from '../core/models';
import { TodoService } from '../core/services/todo.service';
import { UserService } from '../core/services/user.service';

@Component({
  selector: 'app-todo',
  templateUrl: './todo.component.html',
  styleUrls: ['./todo.component.css'],
})
export class TodoComponent implements OnInit {
  errors: Errors = { errors: {} };
  errorMessage: string = '';
  newTodo: Todo = {
    description: '',
    id: 0,
    isActive: 1,
    isCompleted: 0,
    userId: '',
  };
  todoList: Todo[] = [];
  constructor(
    private userService: UserService,
    private todoService: TodoService
  ) {}

  ngOnInit(): void {
    this.todoService.getByUserId().subscribe(
      (data) => (this.todoList = data),
      (err) => {
        this.errors = err;
        this.errorMessage = err.message;
      }
    );
  }

  addTodo() {
    this.todoService.save(this.newTodo).subscribe(
      (data) => {
        this.todoList.push(data);
        this.newTodo = {
          description: '',
          id: 0,
          isActive: 1,
          isCompleted: 0,
          userId: '',
        };
      },
      (err) => {
        this.errors = err;
        this.errorMessage = err.message;
      }
    );
  }

  todoDelete(todo: Todo) {
    this.todoService.delete(todo.id).subscribe(
      (data) => {
        this.todoList.forEach((value, index) => {
          if (value.id == todo.id) this.todoList.splice(index, 1);
        });
      },
      (err) => {
        this.errors = err;
        this.errorMessage = err.message;
      }
    );
  }

  todoComplete(todo: Todo) {
    var isCompleted: number;
    if (todo.isCompleted === 1) isCompleted = 0;
    else isCompleted = 1;
    this.todoService.patch(todo.id, isCompleted).subscribe(
      (data) => {
        todo.isCompleted = data.isCompleted;
      },
      (err) => {
        this.errors = err;
        this.errorMessage = err.message;
      }
    );
  }
}
