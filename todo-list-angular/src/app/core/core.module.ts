import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpTokenInterceptor } from './interceptors';
import { ApiService } from './services/api.service';
import { UserService } from './services/user.service';
import { TodoService } from './services/todo.service';
import { JwtService } from './services/jwt.service';

@NgModule({
  declarations: [],
  imports: [CommonModule],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: HttpTokenInterceptor, multi: true },
    ApiService,
    UserService,
    TodoService,
    JwtService,
  ],
})
export class CoreModule {}
