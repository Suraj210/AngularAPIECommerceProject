import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpStatusCode,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { error } from 'console';
import { Observable, catchError, of } from 'rxjs';
import {
  CustomToastrService,
  ToastrMessageType,
  ToastrPosition,
} from '../ui/custom-toastr.service';
import { UserAuthService } from './models/user-auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from '../../base/base.component';

@Injectable({
  providedIn: 'root',
})
export class HttpErrorHandlerInterceptorService implements HttpInterceptor {
  constructor(
    private toastrService: CustomToastrService,
    private userAuthService: UserAuthService,
    private router: Router,
    private spinner: NgxSpinnerService
  ) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error) => {
        switch (error.status) {
          case HttpStatusCode.Unauthorized:
            this.userAuthService
              .refreshTokenLogin(
                localStorage.getItem('refreshToken'),
                (state) => {
                  if (!state) {
                    const url = this.router.url;
                    if (url == '/products') {
                      this.toastrService.message(
                        'You need to log in to add product.',
                        'Log in',
                        {
                          messageType: ToastrMessageType.Warning,
                          position: ToastrPosition.TopFullWidth,
                        }
                      );
                    } else {
                      this.toastrService.message(
                        'You do not have permition for this action.',
                        'Unathorized Action!',
                        {
                          messageType: ToastrMessageType.Warning,
                          position: ToastrPosition.TopLeft,
                        }
                      );
                    }
                  }
                }
              )
              .then((data) => {});
            break;
          case HttpStatusCode.InternalServerError:
            this.toastrService.message(
              'Lost connection with Server.',
              'Server Error!',
              {
                messageType: ToastrMessageType.Warning,
                position: ToastrPosition.TopLeft,
              }
            );
            break;
          case HttpStatusCode.BadRequest:
            this.toastrService.message(
              'Server cannot process or recognize the request.',
              'UnRecognised Action',
              {
                messageType: ToastrMessageType.Warning,
                position: ToastrPosition.TopLeft,
              }
            );
            break;
          case HttpStatusCode.NotFound:
            this.toastrService.message(
              'Page could not found',
              '404 Not Found',
              {
                messageType: ToastrMessageType.Warning,
                position: ToastrPosition.TopLeft,
              }
            );
            break;

          default:
            this.toastrService.message(
              'Something went wrong please try again later.',
              'Warning!',
              {
                messageType: ToastrMessageType.Warning,
                position: ToastrPosition.TopLeft,
              }
            );
            break;
        }
        this.spinner.hide(SpinnerType.BallAtom);
        return of(error);
      })
    );
  }
}
