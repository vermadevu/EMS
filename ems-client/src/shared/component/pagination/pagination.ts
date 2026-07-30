
import { Component, computed, input, output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  imports: [],
  templateUrl: './pagination.html',
  styleUrl: './pagination.css',
})
export class PaginationComponent {
  readonly pageNumber = input.required<number>();
  readonly pageSize = input.required<number>();
  readonly totalCount = input.required<number>();
  readonly pageChange = output<number>();

  readonly totalPages = computed(() =>
    Math.ceil(this.totalCount() / this.pageSize())
  );

  readonly startItem = computed(() =>
    this.totalCount() === 0
      ? 0
      : (this.pageNumber() - 1) * this.pageSize() + 1
  );

  readonly endItem = computed(() =>
    Math.min(
      this.pageNumber() * this.pageSize(),
      this.totalCount()
    )
  );

  previous() {
    if (this.pageNumber() > 1) {
      this.pageChange.emit(this.pageNumber() - 1);
    }
  }

  next() {
    if (this.pageNumber() < this.totalPages()) {
      this.pageChange.emit(this.pageNumber() + 1);
    }
  }

}
