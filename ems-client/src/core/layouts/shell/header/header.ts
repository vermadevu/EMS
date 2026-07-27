import { Component, inject, output } from '@angular/core';
import { CurrentUserService } from '../../../services/current-user.service';
import { LayoutService } from '../../../services/layout.service';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-header',
  imports: [
    MatIconModule
  ],
  templateUrl: './header.html',
  styleUrl: './header.css',
})
export class Header {
  readonly currentUser = inject(CurrentUserService);
  readonly toggleSidebar = output<void>();
  readonly authService = inject(AuthService);

  readonly layout = inject(LayoutService);

  logout() : void {
    this.authService.logout();
  }
}
