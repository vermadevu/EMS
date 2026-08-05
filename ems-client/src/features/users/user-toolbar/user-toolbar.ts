import { Component, input, output } from '@angular/core';
import { SearchBox } from '../../../shared/components/search-box/search-box';

@Component({
  selector: 'app-user-toolbar',
  standalone: true,
  imports: [
    SearchBox
  ],
  templateUrl: './user-toolbar.html',
  styleUrl: './user-toolbar.css'
})
export class UserToolbarComponent {
  readonly search = input('');
  readonly searchChange = output<string>();
  readonly roles = input<string[]>([]);
  readonly roleChange = output<string | undefined>();
  readonly statusChange = output<boolean | undefined>();

  onRoleChanged(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    this.roleChange.emit(value || undefined);

  }

  onStatusChanged(event: Event): void {
    const value =
      (event.target as HTMLSelectElement).value;
    switch (value) {

      case 'true':
        this.statusChange.emit(true);
        break;

      case 'false':
        this.statusChange.emit(false);
        break;

      default:
        this.statusChange.emit(undefined);
        break;
    }
  }
}