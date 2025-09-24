"use client";

import { Button } from "@/components/ui/button";
import { ChevronLeft, ChevronRight } from "lucide-react";

interface FlashcardNavigationProps {
  currentIndex: number;
  totalCards: number;
  onPrevious: () => void;
  onNext: () => void;
  onGoToCard: (index: number) => void;
}

export default function FlashcardNavigation({
  currentIndex,
  totalCards,
  onPrevious,
  onNext,
  onGoToCard,
}: FlashcardNavigationProps) {
  return (
    <div className="flex justify-between items-center">
      <Button
        onClick={onPrevious}
        disabled={currentIndex === 0}
        variant="outline"
        size="lg"
        className="flex items-center gap-2 bg-transparent"
      >
        <ChevronLeft className="w-4 h-4" />
        Previous
      </Button>

      <div className="flex gap-2">
        {Array.from({ length: totalCards }, (_, index) => (
          <button
            key={index}
            onClick={() => onGoToCard(index)}
            className={`w-3 h-3 rounded-full transition-colors ${
              index === currentIndex
                ? "bg-primary"
                : "bg-muted hover:bg-muted-foreground/20"
            }`}
            aria-label={`Go to card ${index + 1}`}
          />
        ))}
      </div>

      <Button
        onClick={onNext}
        disabled={currentIndex === totalCards - 1}
        variant="outline"
        size="lg"
        className="flex items-center gap-2 bg-transparent"
      >
        Next
        <ChevronRight className="w-4 h-4" />
      </Button>
    </div>
  );
}
