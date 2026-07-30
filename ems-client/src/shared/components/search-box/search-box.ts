import { Component, input, output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-search-box',
  imports: [
    MatIconModule
  ],
  templateUrl: './search-box.html',
  styleUrl: './search-box.css',
})
export class SearchBox {
  readonly value = input('');
  readonly placeholder = input('Search...');
  readonly valueChange = output<string>();

  onInput(event: Event): void {
    const value = (event.target as HTMLInputElement).value;
    this.valueChange.emit(value);
  }

  clear(): void {
    this.valueChange.emit('');
  }

}
