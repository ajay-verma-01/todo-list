import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { Errors } from './core/models';
import { UserService } from './core/services/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'todo-list-angular';
  userId = '';
  password = '';
  errorMessage = '';
  errors: Errors = { errors: {} };
  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    //Important remove the hardcodeing after bulding login page
    //For now I have hardcoded userid and password,
    //later we can create a login page and take the userid and password from login page.
    this.userId = environment.userId;
    this.password = environment.password;
  }

  ngOnInit(): void {
    this.userService.authenticateUser(this.userId, this.password).subscribe(
      (data) => this.router.navigateByUrl('/'),
      (err) => {
        this.errors = err;
        this.errorMessage = err.message;
      }
    );
  }
}
