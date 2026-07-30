import { Component, input, output } from '@angular/core';
import { SearchBox } from '../../../shared/components/search-box/search-box';
import { MatIconModule } from '@angular/material/icon';
import { Department } from '../../../core/models/department';

@Component({
  selector: 'app-employee-toolbar',
  imports: [SearchBox,
    MatIconModule
  ],
  templateUrl: './employee-toolbar.html',
  styleUrl: './employee-toolbar.css',
})
export class EmployeeToolbarComponent {
  readonly search = input('');
  readonly searchChange = output<string>();
  readonly departments = input<Department[]>([]);
  readonly departmentChange = output<number | undefined>();

  onDepartmentChange(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    this.departmentChange.emit(value ? Number(value) : undefined);
  }
}
