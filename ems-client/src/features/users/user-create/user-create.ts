import { Component, inject, OnInit, signal } from '@angular/core';
import { UserService } from '../../../core/services/user-service';
import { AvailableEmployee } from '../../../core/models/available-employee';
import { FormBuilder } from '@angular/forms';
import { UserFormComponent } from '../user-form/user-form';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';

@Component({
  selector: 'app-user-create',
  imports: [
    UserFormComponent,
    PageHeaderComponent
  ],
  templateUrl: './user-create.html',
  styleUrl: './user-create.css',
})
export class UserCreateComponent {
  
}