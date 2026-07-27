import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Header } from '../header/header';
import { Sidebar } from '../sidebar/sidebar';

@Component({
  selector: 'app-shell',
  imports: [RouterOutlet,
    Header,
    Sidebar
  ],
  templateUrl: './shell.html',
  styleUrl: './shell.css',
})
export class Shell {
  readonly collapsed = signal(false);

  toggleSidebar() {
    this.collapsed.update(value => !value);
  }
}
