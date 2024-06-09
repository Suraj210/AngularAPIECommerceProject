import { NgModule } from '@angular/core';
import {
  BrowserModule,
  provideClientHydration,
} from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AdminModule } from './admin/admin.module';
import { UiModule } from './ui/ui.module';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule, provideToastr } from 'ngx-toastr';
import { NgxSpinnerModule } from 'ngx-spinner';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { JwtModule } from '@auth0/angular-jwt';
import { LoginComponent } from './ui/components/login/login.component';
import {
  GoogleLoginProvider,
  SocialAuthServiceConfig,
  SocialLoginModule,
  GoogleSigninButtonModule,
  FacebookLoginProvider,
} from '@abacritt/angularx-social-login';
import { HttpErrorHandlerInterceptorService } from './services/common/http-error-handler-interceptor.service';
import { DynamicLoadComponentDirective } from './directives/common/dynamic-load-component.directive';

@NgModule({
  declarations: [AppComponent, LoginComponent, DynamicLoadComponentDirective],
  imports: [
    GoogleSigninButtonModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AdminModule,
    UiModule,
    ToastrModule.forRoot(), // ToastrModule added
    NgxSpinnerModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: () => localStorage.getItem('accessToken'),
        allowedDomains: ['localhost:7188'],
      },
    }),
    SocialLoginModule,
  ],
  providers: [
    provideClientHydration(),
    provideAnimationsAsync(),
    provideToastr(),
    { provide: 'baseUrl', useValue: 'https://localhost:7188/api', multi: true },
    { provide: 'baseSignalRUrl', useValue: 'https://localhost:7188/', multi: true },
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(
              '1079771414722-e4elofclpef6ukibmahv8ng5tm98le9f.apps.googleusercontent.com'
            ),
          },
          {
            id: FacebookLoginProvider.PROVIDER_ID,
            provider: new FacebookLoginProvider('1146003489971465'),
          },
        ],
        onError: (err) => console.log(err),
      } as SocialAuthServiceConfig,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorHandlerInterceptorService,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
