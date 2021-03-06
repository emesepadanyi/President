import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Credentials } from '../../models/credentials.interface';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.scss']
})

export class LoginFormComponent implements OnInit, OnDestroy {

  private subscription: Subscription;

  brandNew: boolean;
  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;
  credentials: Credentials = { userName: '', password: '' };

  constructor(private userService: UserService, private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.subscription = this.activatedRoute.queryParams.subscribe(
      (param: any) => {
        this.brandNew = param['brandNew'];
        this.credentials.userName = param['userName'];
      });

  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  login({ value, valid }: { value: Credentials, valid: boolean }) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';
    if (valid) {
      this.userService.login(value.userName, value.password)
        .finally(() => this.isRequesting = false)
        .subscribe(
          result => {
            if (result) {
              this.userService.subscribeOnline();
              this.router.navigate(['/dashboard/home']);
            }
          },
          error => this.errors = error);
    }
  }
}
