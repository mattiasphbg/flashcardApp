"use client";

import { useState } from "react";
import { Card } from "@/components/ui/card";

interface FlashcardData {
  id: number;
  question: string;
  answer: string;
}

interface FlashcardProps {
  card: FlashcardData;
  isFlipped: boolean;
  onFlip: () => void;
}

export default function Flashcard({ card, isFlipped, onFlip }: FlashcardProps) {
  const [isAnimating, setIsAnimating] = useState(false);

  const handleFlip = () => {
    if (isAnimating) return;

    setIsAnimating(true);
    setTimeout(() => {
      onFlip();
      setIsAnimating(false);
    }, 300);
  };

  return (
    <div className="relative">
      <Card
        className={`w-full h-80 cursor-pointer transition-all duration-300 hover:shadow-lg border-2 ${
          isAnimating ? "flip-animation" : ""
        }`}
        onClick={handleFlip}
      >
        <div className="h-full flex flex-col items-center justify-center p-8 text-center">
          {!isFlipped ? (
            <div className="space-y-4">
              <div className="text-sm font-medium text-accent uppercase tracking-wide">
                Question
              </div>
              <h2 className="text-2xl font-semibold text-card-foreground text-balance leading-relaxed">
                {card.question}
              </h2>
            </div>
          ) : (
            <div className="space-y-4">
              <div className="text-sm font-medium text-accent uppercase tracking-wide">
                Answer
              </div>
              <p className="text-xl text-card-foreground text-pretty leading-relaxed">
                {card.answer}
              </p>
            </div>
          )}
        </div>
      </Card>
    </div>
  );
}
