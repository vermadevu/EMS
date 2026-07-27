import { Component, inject, output } from '@angular/core';
import { CurrentUserService } from '../../../services/current-user.service';
import { LayoutService } from '../../../services/layout.service';
import { MatIconModule } from '@angular/material/icon';

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

  readonly layout = inject(LayoutService);
}
