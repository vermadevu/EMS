import { Component, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-designation-toolbar',
  imports: [
    MatIconModule,
    FormsModule
  ],
  templateUrl: './designation-toolbar.html',
  styleUrl: './designation-toolbar.css',
})
export class DesignationToolbarComponent {
  readonly search = input('');
  readonly searchChange = output<string>();

  onSearch(value: string) {
    this.searchChange.emit(value);
  }
}
