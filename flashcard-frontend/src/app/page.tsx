"use client";

import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { ChevronLeft, ChevronRight, RotateCcw } from "lucide-react";

const flashcards = [
  {
    id: 1,
    question: "What is the capital of France?",
    answer:
      "Paris is the capital and most populous city of France, known for its art, fashion, gastronomy, and culture.",
  },
  {
    id: 2,
    question: "What is the largest planet in our solar system?",
    answer:
      "Jupiter is the largest planet in our solar system, with a mass more than twice that of all other planets combined.",
  },
  {
    id: 3,
    question: "Who wrote 'Romeo and Juliet'?",
    answer:
      "William Shakespeare wrote 'Romeo and Juliet' around 1594-1596. It's one of his most famous tragedies.",
  },
  {
    id: 4,
    question: "What is the chemical symbol for gold?",
    answer:
      "Au is the chemical symbol for gold, derived from the Latin word 'aurum' meaning gold.",
  },
  {
    id: 5,
    question: "In what year did World War II end?",
    answer:
      "World War II ended in 1945, with Germany surrendering in May and Japan surrendering in September.",
  },
];

export default function FlashcardPage() {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isFlipped, setIsFlipped] = useState(false);
  const [isAnimating, setIsAnimating] = useState(false);

  const currentCard = flashcards[currentIndex];

  const handleFlip = () => {
    if (isAnimating) return;

    setIsAnimating(true);
    setTimeout(() => {
      setIsFlipped(!isFlipped);
      setIsAnimating(false);
    }, 300);
  };

  const handleNext = () => {
    if (currentIndex < flashcards.length - 1) {
      setCurrentIndex(currentIndex + 1);
      setIsFlipped(false);
    }
  };

  const handlePrevious = () => {
    if (currentIndex > 0) {
      setCurrentIndex(currentIndex - 1);
      setIsFlipped(false);
    }
  };

  const resetCard = () => {
    setIsFlipped(false);
  };

  return (
    <div className="min-h-screen bg-background flex flex-col items-center justify-center p-4">
      <div className="w-full max-w-2xl mx-auto space-y-8">
        {/* Header */}
        <div className="text-center space-y-2">
          <h1 className="text-4xl font-bold text-foreground text-balance">
            Study Flashcards
          </h1>
          <p className="text-muted-foreground text-lg">
            Click the card or use the button to reveal answers
          </p>
        </div>

        {/* Card Counter */}
        <div className="text-center">
          <span className="inline-flex items-center px-3 py-1 rounded-full text-sm font-medium bg-secondary text-secondary-foreground">
            {currentIndex + 1} / {flashcards.length}
          </span>
        </div>

        {/* Flashcard */}
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
                    {currentCard.question}
                  </h2>
                </div>
              ) : (
                <div className="space-y-4">
                  <div className="text-sm font-medium text-accent uppercase tracking-wide">
                    Answer
                  </div>
                  <p className="text-xl text-card-foreground text-pretty leading-relaxed">
                    {currentCard.answer}
                  </p>
                </div>
              )}
            </div>
          </Card>
        </div>

        {/* Action Buttons */}
        <div className="flex justify-center gap-4">
          {!isFlipped ? (
            <Button
              onClick={handleFlip}
              className="px-6 py-2 font-medium"
              size="lg"
            >
              Show Answer
            </Button>
          ) : (
            <Button
              onClick={resetCard}
              variant="outline"
              className="px-6 py-2 font-medium bg-transparent"
              size="lg"
            >
              <RotateCcw className="w-4 h-4 mr-2" />
              Show Question
            </Button>
          )}
        </div>

        {/* Navigation */}
        <div className="flex justify-between items-center">
          <Button
            onClick={handlePrevious}
            disabled={currentIndex === 0}
            variant="outline"
            size="lg"
            className="flex items-center gap-2 bg-transparent"
          >
            <ChevronLeft className="w-4 h-4" />
            Previous
          </Button>

          <div className="flex gap-2">
            {flashcards.map((_, index) => (
              <button
                key={index}
                onClick={() => {
                  setCurrentIndex(index);
                  setIsFlipped(false);
                }}
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
            onClick={handleNext}
            disabled={currentIndex === flashcards.length - 1}
            variant="outline"
            size="lg"
            className="flex items-center gap-2 bg-transparent"
          >
            Next
            <ChevronRight className="w-4 h-4" />
          </Button>
        </div>
      </div>
    </div>
  );
}
