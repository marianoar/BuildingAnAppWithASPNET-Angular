import { Component } from '@angular/core';
import { AccountService } from '../services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css'],
})
export class NavComponent {
  model: any = {};
  // currentUser$: Observable<User | null> = of(null); lo uso directo del service

  constructor(
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    // this.currentUser$ = this.accountService.currentUser$;
  }

  // getCurrentUser() {
  //   this.accountService.currentUser$.subscribe({
  //     next: (user) => (this.loggedIn = !!user),
  //     error: (error) => console.log(error),
  //   });
  // }

  login() {
    console.log(this.model);
    this.accountService.login(this.model).subscribe({
      next: (_) => {
       // this.router.navigateByUrl('/members');
        // console.log(response);
        // this.loggedIn = true;
      },
      error: (error) => {
        console.log(error), this.toastr.error(error.error);
      },
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('');
    // this.loggedIn = false;
  }
}
