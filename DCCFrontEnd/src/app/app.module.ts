import { DrugDetailsComponent } from './diagnosis/drug-details/drug-details.component';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
 import { IsEmailExistsDirective } from './register/IsEmailExists.directive';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberProfileEditComponent } from './members/member-profile-edit/member-profile-edit.component';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { DoctorDetailsComponent } from './members/Doctor-Details/Doctor-Details.component';
import { appRouts } from './routes';
 import { AlertifyService } from './_services/alertify.service';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BsDropdownModule, TabsModule, PaginationModule, ButtonsModule } from 'ngx-bootstrap';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { FileUploadModule } from 'ng2-file-upload';

import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';


import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import {HttpClientModule, HTTP_INTERCEPTORS} from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { AuthService } from './_services/Auth.service';
 import { HomeComponent } from './home/home.component';
 import { RegisterComponent } from './register/register.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuardService } from './_guard/auth-guard.service';
import { environment } from 'src/environments/environment';
import {Base_Remote_ApiUrl} from 'src/Config/defaultss.config' ;
import { ApiUrlInterceptor } from './_services/apiurl.interceptor';
import { HttpConfigInterceptors } from './_services/http.interceptor';
import { UserService } from './_services/user.service';
import { DoctorsListComponent } from './members/DoctorsList/DoctorsList.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { NgxGalleryModule } from 'ngx-gallery';
import { PreventUnsavedChanges } from './_guard/prevent-unsaver-changes.guard';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { ChooseServicesComponent } from './choose-services/choose-services.component';
import {
   MatAutocompleteModule,
   MatBadgeModule,
   MatBottomSheetModule,
   MatButtonModule,
   MatButtonToggleModule,
   MatCardModule,
   MatCheckboxModule,
   MatChipsModule,
   MatDatepickerModule,
   MatDialogModule,
   MatDividerModule,
   MatExpansionModule,
   MatGridListModule,
   MatIconModule,
   MatInputModule,
   MatListModule,
   MatMenuModule,
   MatNativeDateModule,
   MatPaginatorModule,
   MatProgressBarModule,
   MatProgressSpinnerModule,
   MatRadioModule,
   MatRippleModule,
   MatSelectModule,
   MatSidenavModule,
   MatSliderModule,
   MatSlideToggleModule,
   MatSnackBarModule,
   MatSortModule,
   MatStepperModule,
   MatTableModule,
   MatTabsModule,
   MatToolbarModule,
   MatTooltipModule,
   MatTreeModule,
 } from '@angular/material';
 import {TimeAgoPipe} from 'time-ago-pipe';
import { ListsResolver } from './_resolvers/lists.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { DiagnosisComponent } from './diagnosis/diagnosis.component';
import { DiagnosisService } from './diagnosis/service/diagnosis.service';
import { ParticlesModule } from 'angular-particle';
import { UserHistoryComponent } from './diagnosis/user-history/user-history.component';

export function tokenGetter() {
   return localStorage.getItem('token');
}
@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MessagesComponent,
      DoctorsListComponent,
      MemberCardComponent,
      DoctorDetailsComponent,
      MemberProfileEditComponent,
      PhotoEditorComponent,
      ChooseServicesComponent,
      TimeAgoPipe,
      IsEmailExistsDirective,
      MemberMessagesComponent,
      DiagnosisComponent,
      DrugDetailsComponent,
      UserHistoryComponent
    ],
   imports: [
      BrowserAnimationsModule,
      BrowserModule,
      HttpClientModule,
      NgxGalleryModule,
      FormsModule,
      MatAutocompleteModule,
      MatBadgeModule,
      MatBottomSheetModule,
      MatButtonModule,
      MatButtonToggleModule,
      MatCardModule,
      MatCheckboxModule,
      MatChipsModule,
      MatDatepickerModule,
      MatDialogModule,
      MatDividerModule,
      MatExpansionModule,
      MatGridListModule,
      MatIconModule,
      MatInputModule,
      MatListModule,
      MatMenuModule,
      MatNativeDateModule,
      MatPaginatorModule,
      MatProgressBarModule,
      MatProgressSpinnerModule,
      MatRadioModule,
      MatRippleModule,
      MatSelectModule,
      MatSidenavModule,
      MatSliderModule,
      MatSlideToggleModule,
      MatSnackBarModule,
      MatSortModule,
      MatStepperModule,
      MatTableModule,
      MatTabsModule,
      MatToolbarModule,
      MatTooltipModule,
      MatTreeModule,
      CarouselModule,
      ReactiveFormsModule,
      FileUploadModule,
       BsDropdownModule.forRoot() ,
       FontAwesomeModule,
       BsDatepickerModule.forRoot(),
       TabsModule.forRoot(),
      RouterModule.forRoot(appRouts),
      ButtonsModule,
      JwtModule.forRoot({
         config: {
            tokenGetter: tokenGetter,
            whitelistedDomains: ['http://thedccapp.com'],
            blacklistedRoutes: ['http://thedccapp.com/api/auht']
         }
      }),
      PaginationModule.forRoot(),
      ParticlesModule
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      AlertifyService,
      AuthGuardService,
      { provide: Base_Remote_ApiUrl, useFactory: getRemoteApiUrl },
      {  provide: HTTP_INTERCEPTORS, useClass: ApiUrlInterceptor, multi: true, deps: [Base_Remote_ApiUrl]},
      HttpConfigInterceptors,
      UserService,
      MemberDetailResolver,
      MemberListResolver,
      MemberEditResolver,
      PreventUnsavedChanges,
      ListsResolver,
      MessagesResolver,
      DiagnosisService

   ],
   entryComponents: [DrugDetailsComponent],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
export function getRemoteApiUrl() {
   return environment.baseRemoteApiUrl ;
 }
