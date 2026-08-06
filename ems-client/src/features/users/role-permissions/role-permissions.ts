import { Component, inject, OnInit, signal } from '@angular/core';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { finalize, Subject } from 'rxjs';
import { RoleListItem } from '../../../core/models/role-list-item';
import { RolePermissionService } from '../../../core/services/role-permission-service';
import { RolePermissionTableComponent } from './role-permission-table/role-permission-table';

@Component({
  selector: 'app-role-permissions',
  imports: [
    PageHeaderComponent,
    RolePermissionTableComponent
  ],
  templateUrl: './role-permissions.html',
  styleUrl: './role-permissions.css',
})
export class RolePermissionsComponent implements OnInit {
  private readonly searchSubject = new Subject<string>();
  readonly loading = signal(false);
  readonly roles = signal<RoleListItem[]>([]);
  private readonly rolePermissionService = inject(RolePermissionService);

  ngOnInit(): void {
    this.loadRoles();
  }

  search(value: string): void {
    this.searchSubject.next(value);
  }

  private loadRoles() {
    this.loading.set(true);
    this.rolePermissionService
      .getRoles()
      .pipe(
        finalize(() =>
          this.loading.set(false)
        )
      )
      .subscribe({
        next: roles => {
          this.roles.set(roles);
        },
        error: console.error
      });
  }
}
