import { Component, inject, input } from '@angular/core';
import { Router } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { EmptyStateComponent } from '../../../../shared/components/empty-state/empty-state';
import { RoleListItem } from '../../../../core/models/role-list-item';

@Component({
  selector: 'app-role-permission-table',
  standalone: true,
  imports: [
    EmptyStateComponent,
    MatIconModule
  ],
  templateUrl: './role-permission-table.html',
  styleUrl: './role-permission-table.css'
})
export class RolePermissionTableComponent {
  readonly roles = input.required<RoleListItem[]>();
  readonly loading = input(false);
  private readonly router = inject(Router);

  openRole(role: RoleListItem) {
    this.router.navigate([
      '/role-permissions',
      role.id
    ]);
  }

}