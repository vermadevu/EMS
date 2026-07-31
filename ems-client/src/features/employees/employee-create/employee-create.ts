import { Component } from '@angular/core';
import { PageHeaderComponent } from '../../../shared/components/page-header/page-header';
import { EmployeeFormComponent } from '../components/employee-form/employee-form';

@Component({
  selector: 'app-employee-create',
  imports: [
    PageHeaderComponent,
    EmployeeFormComponent
  ],
  templateUrl: './employee-create.html',
  styleUrl: './employee-create.css',
})
export class EmployeeCreateComponent {}
