import { AbstractControl } from '@angular/forms';

export function ValidateEmail(control: AbstractControl) {
    const nameRegexp: RegExp = /[!#$%^&*()+\=\[\]{};':"\\|,<>\/?]/;
    if (control.value && nameRegexp.test(control.value)) {
        return { specialCharacter: true };
    }
    return null;
}