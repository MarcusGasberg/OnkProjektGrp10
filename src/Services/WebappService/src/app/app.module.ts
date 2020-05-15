import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, NgModule } from '@angular/core';

import { MatToolbarModule } from '@angular/material/toolbar';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './home/home.component';
import { ToolbarComponent } from './toolbar/toolbar.component';
import { StockListItemComponent } from './stock-list-item/stock-list-item.component';
import { BuySellStockComponent } from './buy-sell-stock/buy-sell-stock.component';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import {
  AuthModule,
  LogLevel,
  OidcConfigService,
  PublicEventsService,
  EventTypes,
} from 'angular-auth-oidc-client';
import { environment } from 'src/environments/environment';
import { AuthInterceptor } from './auth/auth-interceptor';
import { filter } from 'rxjs/operators';

export function configureAuth(oidcConfigService: OidcConfigService) {
  return () =>
    oidcConfigService.withConfig({
      stsServer: environment.authority,
      redirectUrl: environment.redirectUri,
      clientId: environment.clientId,
      responseType: environment.responseType,
      postLogoutRedirectUri: environment.redirectUri,
      autoUserinfo: true,
      scope: environment.scope,
      logLevel: LogLevel.Debug,
    });
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ToolbarComponent,
    StockListItemComponent,
    BuySellStockComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    ScrollingModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatButtonModule,
    HttpClientModule,
    AuthModule.forRoot(),
  ],
  providers: [
    OidcConfigService,
    {
      provide: APP_INITIALIZER,
      useFactory: configureAuth,
      deps: [OidcConfigService],
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {
  constructor(private readonly eventService: PublicEventsService) {
    this.eventService
      .registerForEvents()
      .pipe(
        filter((notification) => notification.type === EventTypes.ConfigLoaded)
      )
      .subscribe((config) => {
        console.log('ConfigLoaded', config);
      });
  }
}
