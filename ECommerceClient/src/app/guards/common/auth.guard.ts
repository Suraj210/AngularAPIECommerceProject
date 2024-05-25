import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import {
  CustomToastrService,
  ToastrMessageType,
  ToastrPosition,
} from '../../services/ui/custom-toastr.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from '../../base/base.component';

export const AuthGuard: CanActivateFn = (route, state) => {
  const router: Router = inject(Router);
  const toastrService: CustomToastrService = inject(CustomToastrService);
  const spinner: NgxSpinnerService = inject(NgxSpinnerService);
  const jwtHelper: JwtHelperService = inject(JwtHelperService);
  const token: string = localStorage.getItem('accessToken');

  spinner.show(SpinnerType.BallAtom);

  // const decodeToken = jwtHelper.decodeToken(token);
  // const expirationDate: Date = jwtHelper.getTokenExpirationDate(token);
  let expired: boolean;
  try {
    expired = jwtHelper.isTokenExpired(token);
  } catch {
    expired = true;
  }

  if (!token || expired) {
    router.navigate(['login'], { queryParams: { returnUrl: state.url } });
    toastrService.message('You need to log in!', 'Unauthorized movement', {
      messageType: ToastrMessageType.Warning,
      position: ToastrPosition.TopLeft,
    });
  }

  spinner.hide(SpinnerType.BallAtom);

  return true;
};
