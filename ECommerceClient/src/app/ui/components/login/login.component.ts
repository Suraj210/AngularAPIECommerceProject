import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../services/common/models/user.service';
import { BaseComponent, SpinnerType } from '../../../base/base.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthService } from '../../../services/common/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FacebookLoginProvider,
  SocialAuthService,
  SocialUser,
} from '@abacritt/angularx-social-login';
import { HttpClientService } from '../../../services/common/http-client.service';
import { UserAuthService } from '../../../services/common/models/user-auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent extends BaseComponent {
  constructor(
    private userAuthService: UserAuthService,
    spinner: NgxSpinnerService,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private socialAuthService: SocialAuthService
  ) {
    super(spinner);
    this.socialAuthService.authState.subscribe(async (user: SocialUser) => {
      debugger;
      console.log(user);
      this.showSpinner(SpinnerType.BallAtom);
      switch (user.provider) {
        case 'GOOGLE':
          await userAuthService.googleLogin(user, () => {
            this.authService.identityCheck();
            this.router.navigate(['']);
            this.hideSpinner(SpinnerType.BallAtom);
          });
          break;
        case 'FACEBOOK':
          await userAuthService.facebookLogin(user, () => {
            this.authService.identityCheck();
            this.router.navigate(['']);
            this.hideSpinner(SpinnerType.BallAtom);
          });
          break;
      }
    });
  }

  async login(userNameOrEmail: string, password: string) {
    this.showSpinner(SpinnerType.BallAtom);
    await this.userAuthService.login(userNameOrEmail, password, () => {
      this.authService.identityCheck();
      this.activatedRoute.queryParams.subscribe((params) => {
        const returnUrl: string = params['returnUrl'];
        if (returnUrl) this.router.navigate([returnUrl]);
        else this.router.navigate(['']);
      });
      this.hideSpinner(SpinnerType.BallAtom);
    });
  }

  async facebookLogin() {
    this.socialAuthService.signIn(FacebookLoginProvider.PROVIDER_ID);
  }
}
