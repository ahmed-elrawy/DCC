import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Directive, forwardRef } from '@angular/core';
import { NG_ASYNC_VALIDATORS, AbstractControl, ValidationErrors } from '@angular/forms';
import { AuthService } from '../_services/Auth.service';

@Directive({
  selector: '[IsEmailExists][formControlName]',
  providers: [
    {
      provide: NG_ASYNC_VALIDATORS,
      useExisting: forwardRef(() => IsEmailExistsDirective),
      multi: true
    }
  ]
})
export class IsEmailExistsDirective {

  constructor(private check:AuthService) { }

  validate(control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
    return this.check.checkEmail(control.value).pipe(
     map((r) => {
        if (r == true) return ({ IsEmailExists: true });
     }))
 }
}
