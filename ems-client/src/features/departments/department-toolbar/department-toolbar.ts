import { Component, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-department-toolbar',
  imports: [
    MatIconModule,
    FormsModule
  ],
  templateUrl: './department-toolbar.html',
  styleUrl: './department-toolbar.css',
})
export class DepartmentToolbarComponent {
  readonly search = input('');
  readonly searchChange = output<string>();

  onSearch(value: string) {
    this.searchChange.emit(value);
  }
}
