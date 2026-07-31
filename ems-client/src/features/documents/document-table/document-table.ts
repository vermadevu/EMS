import { Component, input, output } from '@angular/core';
import { EmployeeDocumentSummary } from '../../../core/models/employee-document-summary';
import { EmptyStateComponent } from '../../../shared/components/empty-state/empty-state';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-document-table',
  imports: [
    EmptyStateComponent,
    MatIconModule
  ],
  templateUrl: './document-table.html',
  styleUrl: './document-table.css',
})
export class DocumentTableComponent {
  readonly employees = input.required<EmployeeDocumentSummary[]>();
  readonly loading = input(false);
  readonly sortBy = input.required<string>();
  readonly sortDirection = input.required<'asc' | 'desc'>();

  readonly employeeSelected = output<number>();
  readonly sortChange = output<string>();

  
  getSortIcon(column: string): string {

    if (this.sortBy() !== column) {
      return 'unfold_more';
    }

    return this.sortDirection() === 'asc'
      ? 'arrow_upward'
      : 'arrow_downward';

  }
}
