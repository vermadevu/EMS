import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  debounceTime,
  distinctUntilChanged,
  finalize,
  Subject
} from 'rxjs';

import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { PaginationComponent } from '../../../shared/component/pagination/pagination';

import { UserToolbarComponent } from '../user-toolbar/user-toolbar';

import { UserService } from '../../../core/services/user-service';
import { ConfirmationService } from '../../../core/services/confirmation-service';
import { NotificationService } from '../../../core/services/notification-service';

import { PagedResult } from '../../employees/models/paged-result';
import { UserTableComponent } from '../user-table/user-table';
import { UserListItem } from '../../../core/models/user-list-item';
import { UserListState } from '../../../core/models/user-list-state';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [
    PageHeaderComponent,
    UserToolbarComponent,
    UserTableComponent,
    PaginationComponent
  ],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css'
})
export class UserListComponent {
  readonly loading = signal(false);
  readonly page = signal<PagedResult<UserListItem> | null>(null);
  readonly users = signal<UserListItem[]>([]);
  readonly roles = signal<string[]>([]);
  private readonly userService = inject(UserService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  private readonly confirmationService =
    inject(ConfirmationService);

  private readonly notificationService =
    inject(NotificationService);

  private readonly searchSubject =
    new Subject<string>();

  readonly state = signal<UserListState>({
    pageNumber: 1,
    pageSize: 10,
    search: ''
  });

  constructor() {
    this.searchSubject
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(search => {
        this.state.update(state => ({
          ...state,
          search,
          pageNumber: 1
        }));

        this.loadUsers();

      });

  }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {
      this.state.update(state => ({
        ...state,
        search: params.get('search') ?? '',
        role: params.get('role') ?? undefined,
        isActive:
          params.get('isActive') === null
            ? undefined
            : params.get('isActive') === 'true',

        pageNumber: params.get('page')
          ? Number(params.get('page'))
          : 1
      }));
      this.loadUsers();
      this.loadRoles();
    });
  }

  loadUsers(): void {
    this.loading.set(true);
    this.userService
      .getUsers(this.state())
      .pipe(
        finalize(() => this.loading.set(false))
      )
      .subscribe({
        next: result => {
          this.page.set(result);
          this.users.set(result.items);
        },
        error: console.error
      });
  }

  search(value: string): void {
    this.searchSubject.next(value);
  }

  changePage(pageNumber: number): void {
    this.state.update(state => ({
      ...state,
      pageNumber
    }));
    this.loadUsers();
  }

  changeRole(role?: string): void {
    this.updateState({
      role
    });
  }

  changeStatus(isActive?: boolean): void {
    this.updateState({
      isActive
    });
  }

  private updateState(
    changes: Partial<UserListState>
  ): void {

    this.state.update(state => ({
      ...state,
      ...changes,
      pageNumber: 1
    }));
    this.loadUsers();
  }

  handleAction(event: {
    action: string;
    user: UserListItem;
  }): void {

    switch (event.action) {
      case 'view':
        this.router.navigate(['/employees', event.user.employeeId]);
        break;

      case 'editRoles':
        console.log('Edit Roles');
        break;

      case 'resetPassword':
        console.log('Reset Password');
        break;

      case 'enable':
        this.confirmationService.confirm({
          title: 'Enable User',
          message: `Enable ${event.user.fullName}?`,
          confirmText: 'Enable',
          confirmButtonClass: 'btn-success'
        })
          .subscribe(result => {
            if (!result) {
              return;
            }
            this.userService
              .activate(event.user.id)
              .subscribe({
                next: () => {
                  this.notificationService.success(
                    'User enabled successfully.'
                  );
                  this.loadUsers();
                },
                error: console.error
              });
          });
        break;

      case 'disable':

        this.confirmationService.confirm({
          title: 'Disable User',
          message: `Disable ${event.user.fullName}?`,
          confirmText: 'Disable',
          confirmButtonClass: 'btn-error'
        })
          .subscribe(result => {
            if (!result) {
              return;
            }
            this.userService
              .deactivate(event.user.id)
              .subscribe({
                next: () => {
                  this.notificationService.success(
                    'User disabled successfully.'
                  );
                  this.loadUsers();
                },
                error: console.error
              });
          });
        break;
    }
  }

  private loadRoles(): void {
    this.userService
      .getRoles()
      .subscribe({
        next: roles => {
          this.roles.set(roles)
        },
        error: console.error
      });
  }

}