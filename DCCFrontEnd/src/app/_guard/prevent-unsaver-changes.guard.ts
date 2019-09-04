import { Injectable } from '@angular/core';
import { MemberProfileEditComponent } from './../members/member-profile-edit/member-profile-edit.component';
import { CanDeactivate } from '@angular/router';
@Injectable()
export class  PreventUnsavedChanges  implements CanDeactivate<MemberProfileEditComponent> {
    canDeactivate(component: MemberProfileEditComponent)  {
        if (component.editForm.dirty) {
            return confirm('Are You Sure You Want To Continue ? Any Unsaver Changes Will Be Lost !') ;
        }
        return true ;
    }

}
