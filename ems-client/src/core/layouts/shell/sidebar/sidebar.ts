import { Component, inject, input } from '@angular/core';
import { NavigationService } from '../../../services/navigation.service';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-sidebar',
  imports: [
    RouterLink,
    RouterLinkActive,
    MatIconModule
  ],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar {
  private readonly navigationService = inject(NavigationService);
  readonly collapsed = input(false);
  readonly navigationItems = this.navigationService.navigationItems;
}
