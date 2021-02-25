import { Component, Input, OnInit } from '@angular/core';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  @Input()
  userId: string | undefined;

  constructor(private userService: UserService) {}

  ngOnInit(): void {}
}
