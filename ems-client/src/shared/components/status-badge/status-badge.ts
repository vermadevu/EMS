import { Component, computed, input } from '@angular/core';

@Component({
  selector: 'app-status-badge',
  imports: [],
  templateUrl: './status-badge.html',
  styleUrl: './status-badge.css',
})
export class StatusBadge {

  readonly status = input.required<string>();

  readonly badgeClass = computed(() => {

    switch (this.status().toLowerCase()) {

      case 'pending':
        return 'badge-warning';

      case 'documentssubmitted':
        return 'badge-info';

      case 'active':
        return 'badge-success';

      case 'inactive':
        return 'badge-error';

      case 'offboarded':
        return 'badge-neutral';

      default:
        return 'badge-outline';

    }
  });
}
