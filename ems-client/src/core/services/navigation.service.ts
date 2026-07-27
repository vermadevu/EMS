import { computed, inject, Service } from '@angular/core';
import { CurrentUserService } from './current-user.service';
import { NAVIGATION_ITEMS } from '../constants/navigation.constants';

@Service()
export class NavigationService {
    private readonly currentUserService = inject(CurrentUserService);
    
    readonly navigationItems = computed(() => {

        return NAVIGATION_ITEMS.filter(item => {

            if (!item.permission) {
                return true;
            }

            return this.currentUserService.hasPermission(item.permission);

        });

    });

}
