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
import {
  AuthService,
  _isAuthenticated,
} from '../../services/common/auth.service';

export const AuthGuard: CanActivateFn = (route, state) => {
  const router: Router = inject(Router);
  const toastrService: CustomToastrService = inject(CustomToastrService);
  const spinner: NgxSpinnerService = inject(NgxSpinnerService);

  if (!_isAuthenticated) {
    router.navigate(['login'], { queryParams: { returnUrl: state.url } });
    toastrService.message('You need to log in!', 'Unauthorized movement', {
      messageType: ToastrMessageType.Warning,
      position: ToastrPosition.TopLeft,
    });
  }

  spinner.hide(SpinnerType.BallAtom);

  return true;
};
