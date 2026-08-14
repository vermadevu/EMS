import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { finalize, switchMap } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { PageHeaderComponent } from '../../../../shared/components/page-header/page-header';
import { RolePermissionService } from '../../../../core/services/role-permission-service';
import { NotificationService } from '../../../../core/services/notification-service';
import { RolePermissionDetails } from '../../../../core/models/role-permission-details';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-role-permission-details',
  standalone: true,
  imports: [
    PageHeaderComponent,
    RouterLink,
    MatIconModule
  ],
  templateUrl: './role-permission-details.html',
  styleUrl: './role-permission-details.css'
})
export class RolePermissionDetailsComponent {

  private readonly rolePermissionService = inject(RolePermissionService);
  private readonly notificationService = inject(NotificationService);
  private readonly authService = inject(AuthService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  readonly loading = signal(true);
  readonly saving = signal(false);

  readonly role = signal<RolePermissionDetails | null>(null);

  ngOnInit(): void {
    const roleId =
      this.route.snapshot.paramMap.get('id');
    if (!roleId) {
      this.router.navigate(['/role-permissions']);
      return;
    }
    this.loadRole(roleId);
  }

  private loadRole(roleId: string): void {
    this.loading.set(true);
    this.rolePermissionService
      .getRolePermissions(roleId)
      .pipe(
        finalize(() =>
          this.loading.set(false)
        )
      )
      .subscribe({
        next: role => {
          this.role.set(role);
        },
        error: () => {
          this.router.navigate([
            '/role-permissions'
          ]);
        }
      });
  }

  togglePermission(permissionId: number, assigned: boolean): void {
    const role = this.role();
    if (!role) {
      return;
    }

    for (const category of role.categories) {
      for (const permission of category.permissions) {
        if (permission.permissionId === permissionId) {
          permission.assigned = assigned;
          break;
        }
      }
      category.assignedPermissions =
        category.permissions.filter(
          x => x.assigned
        ).length;
    }
    this.role.set({
      ...role
    });
  }

  save(): void {
    const role = this.role();
    if (!role) {
      return;
    }

    this.saving.set(true);
    const permissionIds =
      role.categories
        .flatMap(category => category.permissions)
        .filter(permission => permission.assigned)
        .map(permission => permission.permissionId);
    this.rolePermissionService
      .updateRolePermissions(
        role.roleId,
        permissionIds
      )
      .pipe(
        switchMap(() =>
          this.authService.refreshCurrentUser()
        ),
        finalize(() =>
          this.saving.set(false)
        )
      )
      .subscribe({
        next: () => {
          this.notificationService.success(
            'Permissions updated successfully.'
          );
        },
        error: console.error
      });
  }

  assignedPermissions(): number {
    return this.role()?.categories.reduce(
      (sum, category) =>
        sum + category.assignedPermissions,
      0
    ) ?? 0;
  }

  totalPermissions(): number {
    return this.role()?.categories.reduce(
      (sum, category) =>
        sum + category.totalPermissions,
      0
    ) ?? 0;
  }
}