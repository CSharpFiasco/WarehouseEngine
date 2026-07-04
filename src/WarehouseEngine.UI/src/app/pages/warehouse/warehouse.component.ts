import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { WarehouseService } from '../../services/warehouse/warehouse.service';
import type { Warehouse } from '../../types/warehouse';

@Component({
  selector: 'app-warehouse',
  standalone: true,
  imports: [MatTableModule, MatProgressSpinnerModule],
  templateUrl: './warehouse.component.html',
  styleUrl: './warehouse.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WarehouseComponent {
  private readonly warehouseService = inject(WarehouseService);

  protected readonly displayedColumns: readonly string[] = ['id', 'name'];
  protected readonly warehouses = signal<Warehouse[]>([]);
  protected readonly isLoading = signal(true);
  protected readonly error = signal<string | null>(null);

  constructor() {
    this.warehouseService.getAll$().subscribe({
      next: (warehouses) => {
        this.warehouses.set(warehouses);
        this.isLoading.set(false);
      },
      error: (err: unknown) => {
        console.error('Failed to load warehouses', err);
        this.error.set('Failed to load warehouses.');
        this.isLoading.set(false);
      },
    });
  }
}
