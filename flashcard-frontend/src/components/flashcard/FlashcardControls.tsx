"use client";

import { Button } from "@/components/ui/button";
import { RotateCcw } from "lucide-react";

interface FlashcardControlsProps {
  isFlipped: boolean;
  onFlip: () => void;
  onReset: () => void;
}

export default function FlashcardControls({
  isFlipped,
  onFlip,
  onReset,
}: FlashcardControlsProps) {
  return (
    <div className="flex justify-center gap-4">
      {!isFlipped ? (
        <Button onClick={onFlip} className="px-6 py-2 font-medium" size="lg">
          Show Answer
        </Button>
      ) : (
        <Button
          onClick={onReset}
          variant="outline"
          className="px-6 py-2 font-medium bg-transparent"
          size="lg"
        >
          <RotateCcw className="w-4 h-4 mr-2" />
          Show Question
        </Button>
      )}
    </div>
  );
}
