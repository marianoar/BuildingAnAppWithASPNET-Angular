import { CanActivateFn, CanDeactivateFn } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

export const preventUnsavedChangesGuard: CanDeactivateFn<MemberEditComponent> = (component) => {
    if(component.editForm?.dirty){
      return confirm("If continue any unsaved change will be lost");
    }
  return true;
};
