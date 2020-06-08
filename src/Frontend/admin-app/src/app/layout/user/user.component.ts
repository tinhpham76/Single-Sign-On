import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '@app/shared/models/user.model';
import { UserService } from '@app/shared/services/users.services';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {

  
  public users$: Observable<User[]>;
  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.users$ = this.userService.getAll();
  }

}
