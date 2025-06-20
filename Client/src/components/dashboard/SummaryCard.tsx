import React from 'react';
import { DivideIcon as LucideIcon } from 'lucide-react';

interface SummaryCardProps {
  title: string;
  value: number;
  icon: LucideIcon;
  color: 'blue' | 'green' | 'yellow' | 'red';
  trend?: {
    value: number;
    isPositive: boolean;
  };
}

export const SummaryCard: React.FC<SummaryCardProps> = ({
  title,
  value,
  icon: Icon,
  color,
  trend,
}) => {
  const colorClasses = {
    blue: 'bg-blue-50 text-blue-600 border-blue-200',
    green: 'bg-emerald-50 text-emerald-600 border-emerald-200',
    yellow: 'bg-yellow-50 text-yellow-600 border-yellow-200',
    red: 'bg-red-50 text-red-600 border-red-200',
  };

  return (
    <div className="bg-white rounded-xl border border-gray-200 p-6 shadow-sm hover:shadow-md transition-shadow duration-200">
      <div className="flex items-center justify-between">
        <div>
          <p className="text-sm font-medium text-gray-600">{title}</p>
          <p className="text-3xl font-bold text-gray-900 mt-2">{value}</p>
          {trend && (
            <p className={`text-sm mt-2 ${trend.isPositive ? 'text-emerald-600' : 'text-red-600'}`}>
              {trend.isPositive ? '+' : '-'}{Math.abs(trend.value)}% from last week
            </p>
          )}
        </div>
        <div className={`p-3 rounded-lg border ${colorClasses[color]}`}>
          <Icon className="w-6 h-6" />
        </div>
      </div>
    </div>
  );
};