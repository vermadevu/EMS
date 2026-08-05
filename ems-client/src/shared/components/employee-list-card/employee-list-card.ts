import { Component, inject, input } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { EmployeeListItem } from '../../models/employee-list-item';

@Component({
  selector: 'app-employee-list-card',
  imports: [],
  templateUrl: './employee-list-card.html',
  styleUrl: './employee-list-card.css',
})
export class EmployeeListCard {
    readonly title = input.required<string>();
    readonly employees = input.required<EmployeeListItem[]>();
    readonly route = input.required<string>();
    private readonly router = inject(Router);

    go(){
      this.router.navigateByUrl(this.route());
    }
    
}
