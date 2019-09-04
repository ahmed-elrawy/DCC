import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberProfileEditComponent } from './members/member-profile-edit/member-profile-edit.component';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MessagesComponent } from './messages/messages.component';
import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuardService } from './_guard/auth-guard.service';
import { DoctorsListComponent } from './members/DoctorsList/DoctorsList.component';
import { DoctorDetailsComponent } from './members/Doctor-Details/Doctor-Details.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { PreventUnsavedChanges } from './_guard/prevent-unsaver-changes.guard';
import { ChooseServicesComponent } from './choose-services/choose-services.component';
import { RegisterComponent } from './register/register.component';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { DiagnosisComponent } from './diagnosis/diagnosis.component';
import { UserHistoryComponent } from './diagnosis/user-history/user-history.component';
export const appRouts: Routes = [

    {path: 'home' , component: HomeComponent} ,
    {path: 'register' , component: RegisterComponent} ,

    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuardService],
         children: [
            {path: 'messages' , component: MessagesComponent, resolve: {messages:MessagesResolver} } ,
            {path: 'doctors/edit' , component: MemberProfileEditComponent,
             resolve: {user: MemberEditResolver},
              canDeactivate: [PreventUnsavedChanges]} ,

             {path: 'doctors/:id' , component: DoctorDetailsComponent, resolve: {user: MemberDetailResolver} } ,
             {path: 'doctors' , component: DoctorsListComponent, resolve: {users: MemberListResolver}} ,
             {path: 'choose-services' , component: ChooseServicesComponent },
             {path: 'diagnosis' , component: DiagnosisComponent },
             {path: 'my-history' , component: UserHistoryComponent },
             {path: 'diagnosis/drug-details/:id' , component: UserHistoryComponent },



        ]
    },


    {path: '**' , redirectTo: 'home' , pathMatch: 'full'} ,

];
